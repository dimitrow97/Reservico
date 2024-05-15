BEGIN TRANSACTION;

ALTER TABLE [IdentityAuthorizationCodes]
ADD ClientId nvarchar(255) NOT NULL DEFAULT '';

ALTER TABLE [IdentityTokens]
ADD TokenType INT NOT NULL DEFAULT 0;

COMMIT;