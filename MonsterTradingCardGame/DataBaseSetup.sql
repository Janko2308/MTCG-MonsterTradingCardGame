
CREATE TABLE USERS (
	USERNAME VARCHAR(20) PRIMARY KEY,
	PASSWORD VARCHAR(100) NOT NULL,
	COINS INT NOT NULL,
	NAME VARCHAR(20),
	BIO VARCHAR(500),
	IMAGE VARCHAR(50),
	TOKEN VARCHAR(40),
	ROLE INT,
	ELO INT NOT NULL,
	WINS INT NOT NULL,
	LOSSES INT NOT NULL
);

CREATE TABLE CARD (
	ID VARCHAR(100) PRIMARY KEY,
	NAME VARCHAR(30) NOT NULL,
	TYPE VARCHAR(30) NOT NULL,
	ELEMENT VARCHAR(30) NOT NULL,
	DAMAGE INT NOT NULL,
	OWNER VARCHAR(20),
	DECK BOOL
);

CREATE TABLE PACKAGES (
	ID SERIAL PRIMARY KEY,
	CARDID1 VARCHAR(100),
	CARDID2 VARCHAR(100),
	CARDID3 VARCHAR(100),
	CARDID4 VARCHAR(100),
	CARDID5 VARCHAR(100)
);




