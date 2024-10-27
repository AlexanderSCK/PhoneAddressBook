# PhoneAddressBook

# FormBuilder

This is a backend system in C# that enables the creation of person with addresses and phone numbers

Built on .NET 8

Leveraging Postgre database deployed in AWS

## Endpoints

### 1. Get All Persons

- **Endpoint**: GET /api/persons
- **Description**: Retrieves a paginated list of persons with optional filtering by full name.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):

```json
GET /api/persons?pageNumber=1&pageSize=10&filter=John HTTP/1.1
Host: localhost:5000
Accept: application/json
```
- **Response**: (sample):
```json

```

### 2. Get Person By ID
- **Endpoint**: GET /api/persons/{id}
- **Description**: Retrieves detailed information of a specific person by their ID.
- **Headers**:
  - `Content-Type`: application/json
- **Response**: (sample):

```json

```
### 3. Add a New Person
- **Endpoint**: POST /api/persons
- **Description**: Adds a new person along with their addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):

```json

```

- **Response** (sample):

```json

```

### 4. Update an Existing Person
- **Endpoint**: PUT /api/persons/{id}
- **Description**: Updates the details of an existing person, including their addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Request Body** (sample):

### 4. Delete a Person
- **Endpoint**: DELETE /api/persons/{id}
- **Description**:  Deletes a person and all associated addresses and phone numbers.
- **Headers**:
  - `Content-Type`: application/json
- **Response** (sample): 204 NoContent