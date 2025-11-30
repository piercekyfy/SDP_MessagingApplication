CREATE TABLE chat_users (
	chat_id INTEGER NOT NULL REFERENCES chats(id),
	user_uname VARCHAR(16) NOT NULL REFERENCES users(unique_name),
	joined_at TIMESTAMPTZ,
	PRIMARY KEY (chat_id, user_uname)
);