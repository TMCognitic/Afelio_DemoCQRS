﻿using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Tools.Connections.Databases
{
    public static class DbConnectionExtensions
    {
        public static object? ExecuteScalar(this DbConnection dbConnection, string query, bool isStoredProcedure, object? parameters)
        {
            using (DbCommand dbCommand = CreateCommand(dbConnection, query, isStoredProcedure, parameters))
            {
                if (dbConnection.State is not ConnectionState.Open)
                    dbConnection.Open();

                object? result = dbCommand.ExecuteScalar();
                return result is DBNull ? null : result;
            }
        }

        public static int ExecuteNonQuery(this DbConnection dbConnection, string query, bool isStoredProcedure, object? parameters)
        {
            using (DbCommand dbCommand = CreateCommand(dbConnection, query, isStoredProcedure, parameters))
            {
                if (dbConnection.State is not ConnectionState.Open)
                    dbConnection.Open();

                return dbCommand.ExecuteNonQuery();
            }
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this DbConnection dbConnection, string query, bool isStoredProcedure, object? parameters, bool immediately)
            where TResult : new()
        {
            IEnumerable<TResult> result = ExecuteReader<TResult>(dbConnection, query, isStoredProcedure, parameters);

            return (immediately) ? result.ToArray() : result;
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this DbConnection dbConnection, string query, bool isStoredProcedure, Func<IDataRecord, TResult> selector, object? parameters, bool immediately)
        {
            IEnumerable<TResult> result = ExecuteReader(dbConnection, query, isStoredProcedure, selector, parameters);

            return (immediately) ? result.ToArray() : result;
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this DbConnection dbConnection, string query, bool isStoredProcedure, object? parameters)
            where TResult : new()
        {
            using (DbCommand dbCommand = CreateCommand(dbConnection, query, isStoredProcedure, parameters))
            {
                if (dbConnection.State is not ConnectionState.Open)
                    dbConnection.Open();

                using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
                {
                    while (dbDataReader.Read())
                    {
                        yield return dbDataReader.Convert<TResult>();
                    }
                }
            }
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this DbConnection dbConnection, string query, bool isStoredProcedure, Func<IDataRecord, TResult> selector, object? parameters)
        {
            ArgumentNullException.ThrowIfNull(selector);

            using (DbCommand dbCommand = CreateCommand(dbConnection, query, isStoredProcedure, parameters))
            {
                if (dbConnection.State is not ConnectionState.Open)
                    dbConnection.Open();

                using(DbDataReader dbDataReader = dbCommand.ExecuteReader())
                {
                    while(dbDataReader.Read())
                    {
                        yield return selector(dbDataReader);
                    }
                }
            }
        }

        private static DbCommand CreateCommand(DbConnection dbConnection, string query, bool isStoredProcedure, object? parameters)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = query;

            if (isStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            if(parameters is not null)
            {
                Type type = parameters.GetType();

                foreach(PropertyInfo propertyInfo in type.GetProperties())
                {
                    DbParameter dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = propertyInfo.Name;

                    if (propertyInfo.GetMethod is null)
                        throw new InvalidOperationException($"The getter must be public for the property {propertyInfo.Name}");

                    object? value = propertyInfo.GetMethod.Invoke(parameters, null);

                    if(value is DateOnly dateOnly)
                    {
                        value = dateOnly.ToDateTime(default);
                    }

                    dbParameter.Value = value ?? DBNull.Value;
                    dbCommand.Parameters.Add(dbParameter);
                }
            }

            return dbCommand;
        }
    }
}
