CREATE PROCEDURE [dbo].[CSP_Register]
	@Nom NVARCHAR(50),
	@Prenom NVARCHAR(50),
	@Email NVARCHAR(384),
	@Anniversaire DATE,
	@Passwd NVARCHAR(20)
AS
BEGIN
	INSERT INTO [Utilisateur] (Nom, Prenom, Email, Anniversaire, Passwd) OUTPUT inserted.Id VALUES (@Nom, @Prenom, @Email, @Anniversaire, dbo.CSF_HashPasswd(@Passwd));
END
