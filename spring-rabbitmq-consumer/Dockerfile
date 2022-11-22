#FROM tomcat:8-jdk11-openjdk
#
#COPY target/spring-rabbitmq-consumer-0.0.1-SNAPSHOT.war /usr/local/tomcat/webapps/consumer.war

FROM openjdk:11

COPY  target/spring-rabbitmq-consumer-0.0.1-SNAPSHOT.jar /usr/local/tomcat/webapps/consumer.jar

ENTRYPOINT ["java", "-jar", "/usr/local/tomcat/webapps/consumer.jar"]