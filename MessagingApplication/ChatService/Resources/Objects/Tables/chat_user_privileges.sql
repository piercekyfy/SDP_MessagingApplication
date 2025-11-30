CREATE TABLE chat_user_privileges (
	chat_id INTEGER NOT NULL REFERENCES chats(id),
	user_uname VARCHAR(16) NOT NULL REFERENCES users(unique_name),
	privilege INTEGER NOT NULL,
	PRIMARY KEY (chat_id, user_uname, permission)
);