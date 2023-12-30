CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE SCHEMA IF NOT EXISTS sanasoppa;

CREATE TABLE IF NOT EXISTS sanasoppa.game_session
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    start_time TIMESTAMP WITH TIME ZONE,
    end_time TIMESTAMP WITH TIME ZONE,
    CHECK (end_time IS NULL OR start_time <= end_time)
);

CREATE TABLE IF NOT EXISTS sanasoppa.player
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name TEXT NOT NULL,
    game_session_id UUID NOT NULL,
    UNIQUE (name, game_session_id),
    FOREIGN KEY (game_session_id)
        REFERENCES sanasoppa.game_session(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS sanasoppa.round
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    game_session_id UUID NOT NULL,
    round_number INTEGER NOT NULL,
    leader_id UUID NOT NULL,
    UNIQUE (game_session_id, round_number),
    FOREIGN KEY (game_session_id)
        REFERENCES sanasoppa.game_session(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (leader_id)
        REFERENCES sanasoppa.player(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS sanasoppa.word
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    round_id UUID NOT NULL,
    word TEXT NOT NULL,
    UNIQUE (round_id, word),
    FOREIGN KEY (round_id)
        REFERENCES sanasoppa.round(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS sanasoppa.submission
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    round_id UUID NOT NULL,
    player_id UUID NOT NULL,
    guess TEXT NOT NULL,
    UNIQUE (round_id, player_id),
    FOREIGN KEY (round_id)
        REFERENCES sanasoppa.round(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (player_id)
        REFERENCES sanasoppa.player(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS sanasoppa.score
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    round_id UUID NOT NULL,
    player_id UUID NOT NULL,
    points INTEGER NOT NULL,
    UNIQUE (round_id, player_id),
    FOREIGN KEY (round_id)
        REFERENCES sanasoppa.round(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (player_id)
        REFERENCES sanasoppa.player(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS sanasoppa.vote
(
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    round_id UUID NOT NULL,
    voter_id UUID NOT NULL,
    submission_id UUID NOT NULL,
    UNIQUE (round_id, voter_id),
    FOREIGN KEY (round_id)
        REFERENCES sanasoppa.round(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (voter_id)
        REFERENCES sanasoppa.player(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (submission_id)
        REFERENCES sanasoppa.submission(id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);
