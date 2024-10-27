# PhoneAddressBook

# FormBuilder

This is a backend system in C# that enables the creation of person with addresses and phone numbers

Built on .NET 8

Leveraging Postgre database deployed in AWS using Database First Approach

using Serilog for logging to file

## Endpoints

### 1. Get All Persons

- **Endpoint**: GET /api/persons 
- **Description**: Retrieves a paginated list of persons with optional filtering by full name.
- **Headers**:
  - `Content-Type`: application/json
- **Request Exammple** (sample): 

```json
{
"GET /api/persons?pageNumber=1&pageSize=10&filter=Alexander HTTP/1.1"
}
```
- **Response**: (sample):
```json
{
  "page_size": 10,
  "page_number": 1,
  "total_count": 1,
  "total_pages": 1,
  "people": [
    {
      "id": 4,
      "name": "Alexander Petrov",
      "addresses": [
        {
          "type": 2,
          "address": "Makedonia blv",
          "phone_numbers": [
            "0887733123"
          ]
        }
      ]
    }
  ]
}
```

### 2. Get Person By ID
- **Endpoint**: GET /api/persons/{id}  //Example 4
- **Description**: Retrieves detailed information of a specific person by their ID.
- **Headers**:
  - `Content-Type`: application/json
- **Response**: (sample):

```json
{
  "id": 3,
  "name": "Jane Smith",
  "addresses": [
    {
      "type": 1,
      "address": "789 Maple Ave, Springfield",
      "phone_numbers": [
        "555-4321"
      ]
    },
    {
      "type": 2,
      "address": "321 Oak St, Springfield",
      "phone_numbers": [
        "555-6789"
      ]
    }
  ]
}
```
### 3. Add a New Person
- **Endpoint**: POST /api/persons
- **Description**: Adds a new person along with their addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):

```json
{
  "fullName": "Alexander Petrov",
  "addresses": [
    {
      "type": 1,
      "addressDetail": "Tsar Boris 3",
      "phoneNumbers": [
        {
          "number": "0886733123"
        }
      ]
    }
  ]
}
```

- **Response** (sample):

```json
{
  "id": 4,
  "name": "Alexander Petrov",
  "addresses": [
    {
      "type": 1,
      "address": "Tsar Boris 3",
      "phone_numbers": [
        "0886733123"
      ]
    }
  ]
}
```

### 4. Update an Existing Person
- **Endpoint**: PUT /api/persons/{id} //Example 4
- **Description**: Updates the details of an existing person addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):
```json
{
  "addresses": [
    {
      "type": 2,
      "addressDetail": "Makedonia blv",
      "phoneNumbers": [
        {
          "number": "0887733123"
        }
      ]
    }
  ]
}
```

- **Response** (sample):

```json
{
  "id": 4,
  "name": "Alexander Petrov",
  "addresses": [
    {
      "type": 2,
      "address": "Makedonia blv",
      "phone_numbers": [
        "0887733123"
      ]
    }
  ]
}
```
### 4. Delete a Person
- **Endpoint**: DELETE /api/persons/{id}
- **Description**:  Deletes a person and all associated addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Response** (sample): 200 NoContent

```json
{
"Deleted person with id: [id]"
}
```