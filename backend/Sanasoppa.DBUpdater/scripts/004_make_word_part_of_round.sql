ALTER TABLE sanasoppa.round
    ADD COLUMN word TEXT;

DROP TABLE IF EXISTS sanasoppa.word CASCADE;
