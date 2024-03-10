package com.example.auditorium

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class AuthManager {
    private val retrofit = Retrofit.Builder()
        .baseUrl("https://10.0.2.2:7266/")
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    private val apiService = retrofit.create(ApiService::class.java)

    fun authenticateUser(credentials: Credentials, onResult: (AuthResponse?) -> Unit) {
        val call = apiService.loginUser(credentials)
        call.enqueue(object : Callback<AuthResponse> {
            override fun onResponse(call: Call<AuthResponse>, response: Response<AuthResponse>) {
                if (response.isSuccessful) {
                    val authResponse = response.body()
                    onResult(authResponse)
                } else {
                    // Обработка ошибки при аутентификации

                    onResult(null)
                }
            }

            override fun onFailure(call: Call<AuthResponse>, t: Throwable) {
                // Обработка ошибки сети или других ошибок
                onResult(null)
            }
        })
    }
}