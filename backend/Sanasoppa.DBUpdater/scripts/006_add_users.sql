DROP TABLE sanasoppa.vote CASCADE;
DROP TABLE sanasoppa.score CASCADE;
DELETE FROM sanasoppa.game_session;

ALTER TABLE sanasoppa.player
    ADD COLUMN player_guid UUID NOT NULL;

ALTER TABLE sanasoppa.player
    ALTER COLUMN connection_id DROP NOT NULL;
