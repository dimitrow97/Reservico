BEGIN TRANSACTION;

ALTER TABLE [Reservations]
DROP CONSTRAINT [FK_Reservation_LocationId];

ALTER TABLE [Reservations]
DROP COLUMN LocationId;

COMMIT;