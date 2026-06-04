pipeline {
    agent any

    environment {
        SONAR_PROJECT_KEY = 'AutomaticBuild'
        SONAR_TOKEN = credentials('sonarqube-token')
    }

    stages {

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('SonarQube Begin') {
            steps {
                bat '''
                dotnet sonarscanner begin ^
                /k:"%SONAR_PROJECT_KEY%" ^
                /d:sonar.host.url="http://localhost:9000" ^
                /d:sonar.token="%SONAR_TOKEN%"
                '''
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Run xUnit Tests') {
            steps {
                bat 'dotnet test --configuration Release --logger trx'
            }
        }

        stage('SonarQube End') {
            steps {
                bat '''
                dotnet sonarscanner end ^
                /d:sonar.token="%SONAR_TOKEN%"
                '''
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o F:\\Project\\deploy\\AutomaticBuild'
            }
        }

        stage('Restart IIS') {
            steps {
                bat 'iisreset'
            }
        }
    }

    post {
        success {
            echo 'Build, xUnit tests, SonarQube scan and deployment completed successfully.'
        }

        failure {
            echo 'Pipeline failed.'
        }
    }
}