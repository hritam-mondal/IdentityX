# TestTrek

## Description
An open-source online exam platform for seamless, secure, and customizable exam management.

## Features
- User-friendly interface for both exam-takers and administrators
- Secure login and session management
- Customizable exam creation and scheduling
- Real-time exam monitoring
- Detailed results and analytics

## Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/hritam-mondal/TestTrek.git
    ```
2. Navigate to the project directory:
    ```sh
    cd TestTrek.API
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
