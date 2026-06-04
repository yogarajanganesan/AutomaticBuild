pipeline {
    agent any

    environment {
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
                withSonarQubeEnv('SonarQube') {
                    bat '''
                    dotnet sonarscanner begin ^
                    /k:"AutomaticBuild" ^
                    /d:sonar.token="%SONAR_TOKEN%"
                    '''
                }
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

        stage('Quality Gate') {
            steps {
                timeout(time: 10, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
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
            echo 'Build, xUnit tests, SonarQube analysis, deployment and IIS restart completed successfully.'
        }

        failure {
            echo 'Pipeline failed. Check Jenkins console output.'
        }
    }
}