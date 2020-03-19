


# Dropdown
1. GET api/Dropdown/country
2. GET api/Dropdown/state?id=<country_id>
3. GET api/Dropdown/city?id=<state_id>
4. GET api/Dropdown/salutation
5. GET api/Dropdown/addressType
6. GET api/Dropdown/contactType

Returns
{
	id
	name
}

# User
1. GET /api/user
Returns
[
	User
]

2. GET /api/user/{username}
Returns
{
	User
}

3. GET /api/user?search={name}
[
	User
]

4. GET /api/user/{username}/inactive

5. POST /api/user/add
6. PUT /api/user/{username}/update
7. DELETE /api/user/{username}/remove





