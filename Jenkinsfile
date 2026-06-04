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
				where dotnet-sonarscanner || echo SonarScanner not found in PATH
				'''
			}
		}

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('SonarQube Begin') {
			steps {
				withSonarQubeEnv('SonarQube') {
					bat '''
					"C:\\Users\\User\\.dotnet\\tools\\dotnet-sonarscanner.exe" begin ^
					/k:"AutomaticBuild" ^
					/d:sonar.host.url="%SONAR_HOST_URL%" ^
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
                bat 'dotnet test --configuration Release --logger trx --no-build'
            }
        }

        stage('SonarQube End') {
			steps {
				bat '''
				"C:\\Users\\User\\.dotnet\\tools\\dotnet-sonarscanner.exe" end ^
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
            echo 'Build, SonarQube analysis, deployment and IIS restart completed successfully.'
        }

        failure {
            echo 'Pipeline failed. Check Jenkins console output.'
        }
    }
}