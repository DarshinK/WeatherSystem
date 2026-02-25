pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('sonar-token')
    }

    stages {

        stage('Checkout') {
            steps {
                echo 'Checking out code...'
                checkout scm
            }
        }

        stage('Sonar Begin') {
            steps {
                withSonarQubeEnv('SonarServer') {
                    bat """
                    dotnet sonarscanner begin ^
                      /k:"WeatherApp" ^
                      /d:sonar.host.url="%SONAR_HOST_URL%" ^
                      /d:sonar.login="%SONAR_TOKEN%" ^
                      /d:sonar.sources="backend,frontend"
                    """
                }
            }
        }

        stage('Debug Full Structure') {
            steps {
                bat 'dir'
                bat 'dir backend'
                bat 'dir backend\\WeatherSystem'
                bat 'dir backend\\WeatherSystem /s'
            }
        }

        stage('Build Backend') {
            steps {
                echo 'Building .NET Backend...'
                dir('backend/WeatherSystem') {
                    bat 'dotnet restore'
                    bat 'dotnet build --configuration Release'
                }
            }
        }

        stage('Build Frontend') {
            steps {
                echo 'Building Angular Frontend...'
                dir('frontend/weather-ui') {
                    bat 'npm install'
                    bat 'npm run build -- --configuration production'
                }
            }
        }

        stage('Sonar End') {
            steps {
                withSonarQubeEnv('SonarServer') {
                    bat """
                    dotnet sonarscanner end ^
                      /d:sonar.login="%SONAR_TOKEN%"
                    """
                }
            }
        }

        stage('Quality Gate') {
            steps {
                timeout(time: 2, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}