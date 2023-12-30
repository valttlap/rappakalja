ALTER TABLE sanasoppa.game_session
    ADD COLUMN owner_id UUID,
    ADD CONSTRAINT game_session_owner_id_fkey FOREIGN KEY (owner_id) REFERENCES sanasoppa.player(id);
