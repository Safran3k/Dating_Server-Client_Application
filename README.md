# Dating Server-Client Application

This project was developed as part of the **Service-Oriented Programming** course. It is a server-client application that simulates a dating service where users can register, log in, send and receive invitations, and manage their profiles.

## Project Structure

### Server Side

1. **Felhasznalo.cs**:
   - Manages user data, including username and password.
   
2. **Meghivas.cs**:
   - Handles the invitation system, allowing users to send, accept, or reject invitations.
   
3. **ClientManager.cs**:
   - Manages client connections and processes commands such as `LOGIN`, `INVITE`, `ACCEPT`, and `EXIT`.

4. **Program.cs**:
   - The entry point of the server application, which sets up the TCP listener and handles incoming client connections.

5. **datas.xml** and **felhasznalok.xml**:
   - XML files used to store user and invitation data.

### Client Side

1. **Program.cs**:
   - The client application connects to the server via TCP and sends commands. It also handles server responses and displays them to the user.

## How to Run

1. **Clone the repository** to your local machine.
2. **Start the Server**:
   - Open the server-side project in a C# compatible IDE (e.g., Visual Studio).
   - Run the `Program.cs` file to start the server.
3. **Run the Client**:
   - Open the client-side project in the same or a different IDE.
   - Run the `Program.cs` file to start the client.
   - Enter commands as prompted to interact with the server.

## Supported Commands

- **LOGIN**: Log in with an existing username and password.
- **INVITE**: Send an invitation to another user.
- **ACCEPT**: Accept an invitation.
- **EXIT**: Close the client application.

## License

This project is for educational purposes as part of the Service-Oriented Programming course.
