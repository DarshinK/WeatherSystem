pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                echo 'Checking out code...'
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
                echo 'Temporarily Skipping Build'
            }
        }
    }
}