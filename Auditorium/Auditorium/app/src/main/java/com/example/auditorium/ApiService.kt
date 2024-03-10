package com.example.auditorium

import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface ApiService {
    @POST("/api/account/register")
    suspend fun registerUser(@Body user: User): Response<Any>

    @POST("/api/account/login")
    fun loginUser(@Body credentials: Credentials): Call<AuthResponse>
}
