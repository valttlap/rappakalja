ALTER TABLE sanasoppa.game_session
    ADD COLUMN join_code TEXT UNIQUE;

CREATE OR REPLACE FUNCTION sanasoppa.reset_join_code()
RETURNS TRIGGER AS $$
BEGIN
    -- Check if start_time is populated
    IF NEW.start_time IS NOT NULL THEN
        -- Set join_code to NULL
        NEW.join_code := NULL;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_join_code_null
BEFORE INSERT OR UPDATE ON sanasoppa.game_session
FOR EACH ROW
EXECUTE FUNCTION sanasoppa.reset_join_code();

