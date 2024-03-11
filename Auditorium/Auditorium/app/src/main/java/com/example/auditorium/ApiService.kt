package com.example.auditorium

import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Query

interface ApiService {
    @POST("/api/account/register")
    suspend fun registerUser(@Body user: User): Response<Any>

    @POST("/api/account/login")
    fun loginUser(@Body credentials: Credentials): Call<AuthResponse>

    @GET("/api/application/applicationsList/User")
    fun getRequests(
        @Header("userId") userId: String,
        @Query("status") status: Int
    ): Call<RequestListResponse>

    @DELETE("/api/application/application/delete")
    fun deleteApplication(@Body request: DeleteRequest): Call<Any>
}
