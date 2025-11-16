CREATE TABLE users (
	unique_name VARCHAR(16) PRIMARY KEY CHECK(LENGTH(unique_name) >= 6),
	email VARCHAR(50) UNIQUE,
	date_created DATE
);