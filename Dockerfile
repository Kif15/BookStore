# Stage 1: Build
FROM maven:3.9.5-eclipse-temurin-21 AS build

WORKDIR /app

# Copy pom.xml and download dependencies (cached layer)
COPY pom.xml .
RUN mvn dependency:go-offline -B

# Copy source code and build
COPY src ./src
RUN mvn clean package -DskipTests

# Stage 2: Run
FROM eclipse-temurin:21-jre-alpine

WORKDIR /app

# Copy only the JAR from build stage
COPY --from=build /app/target/*.jar app.jar

# Expose port (Render will use PORT env variable)
EXPOSE 8080

# Run the application with dynamic port from environment variable
ENTRYPOINT ["sh", "-c", "java -jar app.jar --server.port=${PORT:-8080}"]