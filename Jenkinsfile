pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('SONAR_TOKEN')
    }

    stages {

        stage('Check Environment') {
            steps {
                bat '''
                whoami
                where dotnet

                if not exist "C:\\Tools\\SonarScanner\\dotnet-sonarscanner.exe" (
                    echo SonarScanner not found
                    exit /b 1
                )
                '''
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('SonarQube Analysis') {
			steps {
				withSonarQubeEnv('SonarQube') {
					bat '''
					C:\\Tools\\SonarScanner\\dotnet-sonarscanner.exe begin ^
					/k:"AutomaticBuild" ^
					/d:sonar.host.url="%SONAR_HOST_URL%" ^
					/d:sonar.token="%SONAR_TOKEN%"
					'''

					bat 'dotnet build --configuration Release --no-restore'

					bat 'dotnet test --configuration Release --logger trx --no-build'

					bat '''
					C:\\Tools\\SonarScanner\\dotnet-sonarscanner.exe end ^
					/d:sonar.token="%SONAR_TOKEN%"
					'''
				}
			}
		}

		stage('Quality Gate') {
			steps {
				timeout(time: 5, unit: 'MINUTES') {
					waitForQualityGate abortPipeline: true
				}
			}
		}

		stage('Publish') {
			steps {
				 script {
					def outputDir = ""

					if (env.BRANCH_NAME == "main") {
						outputDir = "F:\\Project\\deploy\\AutomaticBuild"
					}
					else if (env.BRANCH_NAME == "feature/Dev") {
						outputDir = "F:\\Project\\deploy\\AutomaticBuild - Dev"
					}
					else if (env.BRANCH_NAME == "feature/QA") {
						outputDir = "F:\\Project\\deploy\\AutomaticBuild - QA"
					}
					else if (env.BRANCH_NAME == "feature/staging") {
						outputDir = "F:\\Project\\deploy\\AutomaticBuild - Staging"
					}

					bat "dotnet publish -c Release -o ${outputDir}"
				}
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
            echo 'Build, SonarQube analysis, deployment and IIS restart completed successfully.'
        }

        failure {
            echo 'Pipeline failed. Check Jenkins console output.'
        }
    }
}