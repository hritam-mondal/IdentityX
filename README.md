# IdentityX

## Description
This is the Auth project, which provides authentication and authorization services for the application. The project includes features for user registration, login, and role-based access control using JWT authentication.

## Features
- User Registration
- User Login
- JWT Authentication
- Role-based Authorization
- Secure Password Storage using BCrypt

## Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/hritam-mondal/IdentityX.git
    ```
2. Navigate to the project directory:
    ```sh
    cd IdentityX.API
    ```
3. Install dependencies and set up the environment:
    ```sh
    dotnet restore
    dotnet build
    ```

## Configuration
Before running the application, you'll need to set up your environment variables for database connection strings and JWT keys.

### Using Secret Manager (for local development)
1. **Initialize user secrets**:
    ```sh
    dotnet user-secrets init
    ```
2. **Add the secrets**:
    ```sh
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=<database_server_ip>;uid=<db_username>;pwd=<db_password>;database=<database_name>"
    dotnet user-secrets set "Jwt:Key" "<your_jwt_secret_key>"
    ```

### Using Environment Variables (for production)
1. **Set the environment variables**:
    ```sh
    export ASPNETCORE_ConnectionStrings__DefaultConnection="server=<database_server_ip>;uid=<db_username>;pwd=<db_password>;database=<database_name>"
    export ASPNETCORE_Jwt__Key="<your_jwt_secret_key>"
    ```

## Usage
1. Run the application:
    ```sh
    dotnet run
    ```
2. Access the platform in your web browser at `http://localhost:5000`.

## Contributing
We welcome contributions! Please fork the repository and submit pull requests.

## License
This project is licensed under the MIT License.
