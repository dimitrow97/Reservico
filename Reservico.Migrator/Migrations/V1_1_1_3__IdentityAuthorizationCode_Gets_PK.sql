BEGIN TRANSACTION;

ALTER TABLE [IdentityAuthorizationCodes]
ADD CONSTRAINT PK_IdentityAuthorizationCodes PRIMARY KEY (Id);

ALTER TABLE [IdentityTokens]
ADD CONSTRAINT PK_IdentityTokens PRIMARY KEY (Id);

COMMIT;