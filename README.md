# IdentityX

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Overview

The Auth project is designed to provide robust and secure authentication and authorization services for any application. It includes features for user registration, login, and role-based access control, leveraging JWT for secure token-based authentication.

## Features

- **User Registration**: Allows new users to create accounts.
- **User Login**: Secure login functionality for existing users.
- **Role-Based Access Control**: Restrict access to resources based on user roles.
- **JWT Authentication**: Secure token-based authentication using JSON Web Tokens.

## Technologies

The Auth project is built using the following technologies:

- **C#** (90.7%)
- **TypeScript** (4.4%)
- **JavaScript** (3.3%)
- **HTML** (1.6%)

## Installation

To get started with the Auth project, follow these steps:

1. **Clone the repository**:
    ```bash
    git clone https://github.com/hritam-mondal/IdentityX.git
    cd IdentityX
    ```

2. **Install dependencies**:
    ```bash
    # For backend
    dotnet restore

    # For frontend
    npm install
    ```

3. **Build the project**:
    ```bash
    # For backend
    dotnet build

    # For frontend
    npm run build
    ```

## Configuration

Before running the application, you'll need to set up your environment variables for database connection strings and JWT keys.

### Using Secret Manager (for local development)

1. **Initialize user secrets**:
    ```bash
    dotnet user-secrets init
    ```

2. **Add the secrets**:
    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=<database_server_ip>;uid=<db_username>;pwd=<db_password>;database=<database_name>"
    dotnet user-secrets set "Jwt:Key" "<your_jwt_secret_key>"
    ```

### Using Environment Variables (for production)

1. **Set the environment variables**:
    ```bash
    export ASPNETCORE_ConnectionStrings__DefaultConnection="server=<database_server_ip>;uid=<db_username>;pwd=<db_password>;database=<database_name>"
    export ASPNETCORE_Jwt__Key="<your_jwt_secret_key>"
    ```

## Usage

To use the Auth project, follow the instructions below:

1. **Run the application**:
    ```bash
    dotnet run
    ```

2. **Access the platform**: Open your web browser and navigate to [http://localhost:5000](http://localhost:5000).

3. **Register a new user**: Use the registration endpoint to create a new user account.
4. **Log in**: Use the login endpoint to authenticate the user and receive a JWT token.
5. **Access protected resources**: Include the JWT token in the Authorization header to access protected endpoints based on user roles.

## Contributing

We welcome contributions from the community. To contribute to the Auth project, follow these steps:

1. **Fork the repository**: Click on the 'Fork' button at the top right of the repository page.
2. **Clone your fork**:
    ```bash
    git clone https://github.com/<your-username>/IdentityX.git
    cd IdentityX
    ```
3. **Create a new branch**:
    ```bash
    git checkout -b feature/your-feature-name
    ```
4. **Make your changes**: Implement your feature or fix.
5. **Commit your changes**:
    ```bash
    git commit -m "Add feature/fix description"
    ```
6. **Push to your fork**:
    ```bash
    git push origin feature/your-feature-name
    ```
7. **Create a Pull Request**: Open a pull request to merge your changes into the main repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

For any questions or inquiries, please contact:
- **Hritam Mondal** - [hritam.mondal@example.com](mailto:hritam.mondal@yahoo.com)
