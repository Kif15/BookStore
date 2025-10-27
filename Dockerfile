FROM eclipse-temurin:21-jre

WORKDIR /app

# Copy the JAR file
COPY *.jar app.jar

# Expose port 8081
EXPOSE 8081

# Run the application
ENTRYPOINT ["java", "-jar", "app.jar"]