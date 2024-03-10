package com.example.auditorium


data class User(
    val fullName: String,
    val email: String,
    val password: String,
    val birthDate: String,
    val gender: String,
    val phoneNumber: String,
    val facultyId: String,
)


data class Credentials(
    val email:  String,
    val password: String
)

data class AuthResponse(
    val token: String
)
