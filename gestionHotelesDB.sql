CREATE SEQUENCE public.geshUsers_usuId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.geshRoles_rolId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.geshCity_cityId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.geshDepartament_depId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.geshContry_countId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.documentType_tipId_seq INCREMENT BY 1 START 1;

CREATE SEQUENCE public.geshHotels_hotId_seq INCREMENT BY 1 START 1;


CREATE TABLE public.geshCity
(
	cityId INTEGER DEFAULT nextval('geshCity_cityId_seq'::regclass) NOT NULL,
	cityName VARCHAR NOT NULL,
	cityidDepartament INTEGER NOT NULL
) WITHOUT OIDS;

/* Add Primary Key */
ALTER TABLE public.geshCity ADD CONSTRAINT geshCity_pkey
PRIMARY KEY (cityId);


CREATE TABLE public.geshDepartament
(
	depId INTEGER DEFAULT nextval('geshDepartament_depId_seq'::regclass) NOT NULL,
	depContryId INTEGER NOT NULL,
	name VARCHAR NOT NULL
) WITHOUT OIDS;

/* Add Primary Key */
ALTER TABLE public.geshDepartament ADD CONSTRAINT geshDepartament_pkey
	PRIMARY KEY (depId);
	
	
	CREATE TABLE public.Contry
(
	countId INTEGER DEFAULT nextval('geshContry_countId_seq'::regclass) NOT NULL,
	countName VARCHAR NOT NULL
) WITHOUT OIDS;

/* Add Primary Key */
ALTER TABLE public.Contry ADD CONSTRAINT contry_pkey
	PRIMARY KEY (countId);
	
	
/* Build Table Structure */
CREATE TABLE public.geshUsers
(
	usuId INTEGER DEFAULT nextval('public.geshUsers_usuId_seq'::regclass) NOT NULL,
	usuNames VARCHAR NOT NULL,
	usuAddress VARCHAR NULL,
	usuPhone VARCHAR NULL,
	usuEmail VARCHAR NULL,
	usuDocumentId VARCHAR NULL,
	usuPassword VARCHAR NULL,
	usuTdocId INTEGER NULL,
	usuRolId INTEGER NULL,
	usuIsActive BOOLEAN NOT NULL DEFAULT TRUE
) WITHOUT OIDS;

ALTER TABLE public.geshUsers ADD CONSTRAINT geshUsers_pkey
	PRIMARY KEY (usuId);
	
	

	
/* Build Table Structure */
CREATE TABLE public.documentType
(
	tipId INTEGER DEFAULT nextval('public.documentType_tipId_seq'::regclass) NOT NULL,
	tipDescription VARCHAR NULL,
	tipIsActive BOOLEAN OT NULL DEFAULT TRUE
) WITHOUT OIDS;

ALTER TABLE public.documentType ADD CONSTRAINT documentType_pkey
	PRIMARY KEY (tipId);
	
	
	CREATE TABLE public.geshRoles
(
	rolId INTEGER DEFAULT nextval('public.geshRoles_rolId_seq'::regclass) NOT NULL,
	rolName  VARCHAR NULL,
	rolObservation VARCHAR NULL,
	rolIsActive BOOLEAN 
) WITHOUT OIDS;

ALTER TABLE public.geshRoles ADD CONSTRAINT geshRoles_pkey
	PRIMARY KEY (rolId);
	
	

/* Build Table Structure */
CREATE TABLE public.geshHotels
(
	hotId INTEGER DEFAULT nextval('public.geshHotels_hotId_seq'::regclass) NOT NULL,
	hotName VARCHAR NOT NULL,
	hotDescription VARCHAR NOT NULL,
	hotAddress VARCHAR NULL,
	hotPhone VARCHAR NULL,
	hotEmail VARCHAR NULL,
	hotCityId  INTEGER NULL,
	hotRating INTEGER NULL,
	hotRateSign VARCHAR NULL,
	hotImages VARCHAR NULL,
	hotIsActive BOOLEAN NOT NULL DEFAULT TRUE
	


) WITHOUT OIDS;

ALTER TABLE public.geshHotels ADD CONSTRAINT geshHotels_pkey
	PRIMARY KEY (hotId);




ALTER TABLE public.geshUsers
ADD CONSTRAINT fk_geshUsers_geshRoles
FOREIGN KEY (usuRolId)
REFERENCES public.geshRoles (rolId)
ON UPDATE NO ACTION ON DELETE NO ACTION;



