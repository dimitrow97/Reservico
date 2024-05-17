BEGIN TRANSACTION;

ALTER TABLE [Reservations]
ADD IsConfirmed bit NOT NULL;

ALTER TABLE [Tables]
ADD WorkingHoursFrom int NOT NULL;

ALTER TABLE [Tables]
ADD WorkingHoursTo int NOT NULL;

ALTER TABLE [Tables]
ADD CanTableTurn bit NOT NULL;

COMMIT;