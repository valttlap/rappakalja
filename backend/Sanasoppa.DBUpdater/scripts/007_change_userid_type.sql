ALTER TABLE sanasoppa.player
    ALTER COLUMN player_guid TYPE TEXT; -- Change the data type from UUID to TEXT

ALTER TABLE sanasoppa.player
    RENAME COLUMN player_guid TO player_id; -- Rename the column to player_id
