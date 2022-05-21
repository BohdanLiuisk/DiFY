-- Add Test Administrator
INSERT INTO users.UserRegistrations VALUES 
(
	'4065630E-4A4C-4F01-9142-0BACF6B8C64D',
	'billyherrington',
	'billyherrington@mail.com',
	'AK0qplH5peUHwnCVuzW9zy0JGZTTG6/Ji88twX+nw9JdTUwqa3Wol1K4m5aCG9pE2A==', -- testAdminPass
	'Billy',
	'Herrington',
	'Billy Herrington',
	'Confirmed',
	GETDATE(),
	GETDATE()
)

INSERT INTO users.Users VALUES
(
	'4065630E-4A4C-4F01-9142-0BACF6B8C64D',
	'billyherrington',
	'billyherrington@mail.com',
	'AK0qplH5peUHwnCVuzW9zy0JGZTTG6/Ji88twX+nw9JdTUwqa3Wol1K4m5aCG9pE2A==', -- testAdminPass
	1,
	'Billy',
	'Herrington',
	'Billy Herrington'
)

INSERT INTO users.UserRoles VALUES
('4065630E-4A4C-4F01-9142-0BACF6B8C64D', 'Administrator')
