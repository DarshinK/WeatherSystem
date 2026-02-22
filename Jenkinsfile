pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                echo 'Checking out code...'
            }
        }
        stage('Debug Files') {
            steps {
                bat 'dir'
                bat 'dir backend'
                bat 'dir backend\\WeatherSystem'
            }
        }
        stage('Build Backend') {
            steps {
                echo 'Building .NET Backend...'
                bat 'dotnet restore backend/WeatherSystem/WeatherSystem.sln'
                bat 'dotnet build backend/WeatherSystem/WeatherSystem.sln --configuration Release'
            }
        }

        stage('Build Frontend') {
            steps {
                echo 'Building Angular Frontend...'
                dir('frontend/weather-app') {
                    bat 'npm install'
                    bat 'npm run build'
                }
            }
        }

        stage('Success') {
            steps {
                echo 'Build completed successfully!'
            }
        }
    }
}