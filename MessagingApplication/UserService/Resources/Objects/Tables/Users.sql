CREATE TABLE users (
	unique_name VARCHAR(16) PRIMARY KEY CHECK(LENGTH(unique_name) >= 6),
	display_name VARCHAR(24) NOT NULL CHECK(LENGTH(TRIM(display_name)) > 0),
	email VARCHAR(50) UNIQUE,
	deleted BOOLEAN DEFAULT(FALSE),
	created_at TIMESTAMPTZ,
	deleted_at TIMESTAMPTZ
);