CREATE DATABASE IF not EXISTS treasured_paradise
	CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE treasured_paradise;

CREATE TABLE players (
	player_id	CHAR(36)	PRIMARY KEY DEFAULT (UUID()),
	name	VARCHAR(64)	NOT NULL,
    currency	DECIMAL(12,2)	NOT NULL DEFAULT 0.00,
    oxygen_level	FLOAT	NOT NULL DEFAULT 100.0,
    oxygen_capactiy FLOAT	NOT NULL DEFAULT 100.0,
    relationship_milestone		INT	NOT NULL DEFAULT 0,
    created_at	TIMESTAMP	DEFAULT CURRENT_TIMESTAMP,
    updated_at	TIMESTAMP	DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    );

CREATE TABLE item_definitions (
	item_def_id	INT 	AUTO_INCREMENT PRIMARY KEY,
    item_key	VARCHAR(64)	NOT NULL UNIQUE,
    display_name	VARCHAR(128) NOT NULL,
    item_type	ENUM('Junk', 'Treasure', 'Artifact', 'GearPiece', 'QuestItem') NOT NULL,
    base_value	DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    gear_slot	VARCHAR(32) NULL,
    lore_index	INT NULL,
    description TEXT NULL,
    FOREIGN KEY (lore_index) REFERENCES lore_entries(lore_id));