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

data class RequestItem(
    val id: String,
    val date: String,
    val ownerId: String,
    val ownerName: String,
    val ownerRole: String,
    val keyId: String,
    val keyNumber: String,
    val repetitive: Boolean,
    val pairStart: String,
    val status: Int
)

data class DeleteRequest(val applicationId: String)

