pipeline {
    agent any

    stages {

        stage('Build') {
            steps {
                bat 'dotnet build'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o C:\\Deploy\\AutomaticBuild'
            }
        }

        stage('Restart IIS') {
            steps {
                bat 'iisreset'
            }
        }
    }
}