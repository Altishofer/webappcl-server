# Word2Vec - Class Room Game

🎉 This repository contains a basic implementation for an interactive classroom-game using Googles Word2Vec implementation. 🎮

---

This project aims to create a dynamic and engaging learning experience for participants by challenging their word vector manipulation skills. Contestants compete in rounds where they're given a target word and a set of forbidden words. The goal is to construct formulas using words (represented as vectors) and mathematical operations, aiming to get as close to the target word as possible without using any of the banned words. The contestant with the formula closest to the target word emerges victorious in each round.

---

### Features

- Hosts (teachers, presenter) can register and create quizzes with multiple rounds, each featuring a unique target word and specific constraints on word usage
- Participants (students, attendees) can register for quizzes via a QR-code, join games, and submit their formulas during each round under a time limit
- Hosts have the ability to start quizzes, broadcast rounds, and receive live answers from players in real-time
- The server allows blazing fast verification of words to always make sure the used words exist in the vector database

### Technology Stack

- PostgreSQL database for storing hosts, players, quizzes, rounds, and results
- Googles Word2Vec binary files for very fast vector calculations using in-memory caching
- RESTful APIs for not time critical communication between the client and the server
- Websockets for real-time communication between the clients and to push updates to the clients
- ASP.NET Core framework for server configuration
- Docker for containerization for local development and stable deployment to Dokku
- Microsoft Entity Framework for leveraged object oriented programming with a database mapper

---

### Prerequisites

Before getting started, make sure you have the following prerequisites:

- [.NET 7.0](https://dotnet.microsoft.com/en-us/download)
- [Angular](https://angular.io/)
- [Docker](https://www.docker.com/)

### Installation

Follow these steps to set up the project:

1. Install prequisites
2. Clone this repository to your local machine:

   ```shell
   git clone https://gitlab.uzh.ch/ltwa-hs23/sandrinraphael.hunkeler/webappcl-server.git
   ```

3. Change into the cloned repository directory:

   ```shell
   cd webappcl-server
   ```

4. Add .NET User Secrets and replace all secrets with your own values:

   ```shell
   {
   "DB_NAME": "",
   "DB_PASSWORD": "",
   "DB_USER": "",
   "DB_HOST": "",
   "DB_PORT": "",
   "JWT_SETTINGS_ISSUER": "",
   "JWT_SETTINGS_AUDIENCE": "",
   "JWT_SETTINGS_KEY": "",
   "VECTOR_BIN": "./GoogleNews-vectors-negative300.bin"
   }
    ```

5. Build & start the web server:

   ```shell
   dotnet run --launch-profile http --environment Development --project ToX
   ```

6. Open your favorite browser and have a look at Swagger! 🎉
   ```shell
   http://localhost:5072/swagger/index.html
   ```


### Simulate remote deployment on Dokku
Verify build with Docker to simulate Dokku deployment
1. Make sure Docker Desktop is running

2. Build the docker image
```shell
docker build -t webappcl-server:latest .
  ```

3. Deploy the docker image

```shell
docker run -d -p 5000:5000 --name webappcl-server webappcl-server:latest
```
