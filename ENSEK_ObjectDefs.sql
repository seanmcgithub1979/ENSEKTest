-- Order matters because of FK relationship
IF EXISTS (SELECT 1 FROM sys.sysobjects WHERE name = 'AccountMeterReading' AND type = 'U')
	DROP TABLE dbo.AccountMeterReading;
GO
IF EXISTS (SELECT 1 FROM sys.sysobjects WHERE name = 'Account' AND type = 'U')
	DROP TABLE dbo.Account;
GO
CREATE TABLE dbo.Account
(
	AccountId				INT	NOT NULL,
	FirstName				NVARCHAR(255),
	LastName				NVARCHAR(255),
	CONSTRAINT	PK_Account_AccountId PRIMARY KEY CLUSTERED (AccountId)
);
GO

CREATE TABLE dbo.AccountMeterReading
(
	AccountMeterReadingId	INT NOT NULL IDENTITY(1,1),
	AccountId				INT	NOT NULL,
	MeterReadingDateTime	DATETIME,
	MeterReadingValue		iNT
	CONSTRAINT	PK_AccountMeterReading_AccountMeterReadingId PRIMARY KEY CLUSTERED (AccountMeterReadingId),
	CONSTRAINT FK_Account_AccountId FOREIGN KEY (AccountId) REFERENCES Account(AccountId) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT 1 FROM sys.sysobjects WHERE name = 'Insert_AccountMeterReading' AND type = 'P')
	DROP PROCEDURE dbo.Insert_AccountMeterReading;
GO
CREATE PROCEDURE dbo.Insert_AccountMeterReading
(
	@AccountId				INT,
	@MeterReadingDateTime	DATETIME,
	@MeterReading			INT
)
AS
DECLARE @ReturnMsg		NVARCHAR(255) = ''
IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE AccountId = @AccountId)
BEGIN
	SELECT @ReturnMsg = N'NO ACCOUNT FOUND FOR: ' + @AccountId;
	RAISERROR (@ReturnMsg, 10, 1);
	RETURN 0;
END
ELSE
BEGIN

	DECLARE @MaxReadingDateTime DATETIME;
	-- Check for newer read
	SELECT @MaxReadingDateTime= MAX(MeterReadingDateTime) FROM dbo.AccountMeterReading WHERE AccountId = @AccountId;

	IF (@MaxReadingDateTime IS NOT NULL AND @MaxReadingDateTime > @MeterReadingDateTime)
	BEGIN
		SELECT @ReturnMsg = N'A NEWER READING EXISTS FOR SAME ACCOUNT: ' + CONVERT(NVARCHAR(255), @AccountId);
		RAISERROR (@ReturnMsg, 10, 1);
		RETURN 0;
	END
	
	IF NOT EXISTS(SELECT 1 FROM dbo.AccountMeterReading WHERE AccountId = @AccountId AND MeterReadingDateTime = @MeterReadingDateTime AND MeterReadingValue = @MeterReading)
	BEGIN
		INSERT INTO dbo.AccountMeterReading(AccountId, MeterReadingDateTime, MeterReadingValue)
		SELECT	@AccountId, @MeterReadingDateTime, @MeterReading;
	END
	ELSE
	BEGIN
		SELECT @ReturnMsg = N'READING EXISTS FOR SAME ACCOUNT AND DATETIME: ' + CONVERT(NVARCHAR(255), @AccountId) + ', ' + CONVERT(NVARCHAR(255), @MeterReadingDateTime);
		RAISERROR (@ReturnMsg, 10, 1);
		RETURN 0;
	END
END
GO

/*
DELETE FROM  Account;
GO

INSERT INTO Account SELECT 2344,'Tommy','Test'
INSERT INTO Account SELECT 2233,'Barry','Test'
INSERT INTO Account SELECT 8766,'Sally','Test'
INSERT INTO Account SELECT 2345,'Jerry','Test'
INSERT INTO Account SELECT 2346,'Ollie','Test'
INSERT INTO Account SELECT 2347,'Tara','Test'
INSERT INTO Account SELECT 2348,'Tammy','Test'
INSERT INTO Account SELECT 2349,'Simon','Test'
INSERT INTO Account SELECT 2350,'Colin','Test'
INSERT INTO Account SELECT 2351,'Gladys','Test'
INSERT INTO Account SELECT 2352,'Greg','Test'
INSERT INTO Account SELECT 2353,'Tony','Test'
INSERT INTO Account SELECT 2355,'Arthur','Test'
INSERT INTO Account SELECT 2356,'Craig','Test'
INSERT INTO Account SELECT 6776,'Laura','Test'
INSERT INTO Account SELECT 4534,'JOSH','Test'
INSERT INTO Account SELECT 1234,'Freya','Test'
INSERT INTO Account SELECT 1239,'Noddy','Test'
INSERT INTO Account SELECT 1240,'Archie','Test'
INSERT INTO Account SELECT 1241,'Lara','Test'
INSERT INTO Account SELECT 1242,'Tim','Test'
INSERT INTO Account SELECT 1243,'Graham','Test'
INSERT INTO Account SELECT 1244,'Tony','Test'
INSERT INTO Account SELECT 1245,'Neville','Test'
INSERT INTO Account SELECT 1246,'Jo','Test'
INSERT INTO Account SELECT 1247,'Jim','Test'
INSERT INTO Account SELECT 1248,'Pam','Test'
GO
*/

/*
SELECT * FROM dbo.Account;
SELECT * FROM dbo.AccountMeterReading;

delete from AccountMeterReading;
*/


