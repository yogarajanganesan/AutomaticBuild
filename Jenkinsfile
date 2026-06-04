pipeline {
    agent any

    environment {
        SONARQUBE_ENV = withSonarQubeEnv('SonarQube')
        SONAR_PROJECT_KEY = 'DotNetCore-IIS-Deploy'
        SONAR_TOKEN = credentials('GlobalJenkinToken')
    }

    stages {

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test --configuration Release --logger trx'
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv("${SONARQUBE_ENV}") {
                    bat """
                    dotnet sonarscanner begin /k:"%SONAR_PROJECT_KEY%" /d:sonar.token="%SONAR_TOKEN%"
                    dotnet build --configuration Release
                    dotnet sonarscanner end /d:sonar.token="%SONAR_TOKEN%"
                    """
                }
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
        always {
            echo 'Pipeline completed.'
        }

        success {
            echo 'Build, Test, SonarQube Analysis and Deployment completed successfully.'
        }

        failure {
            echo 'Pipeline failed. Check Jenkins console output.'
        }
    }
}