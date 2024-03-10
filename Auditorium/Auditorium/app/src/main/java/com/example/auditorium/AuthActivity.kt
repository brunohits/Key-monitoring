package com.example.auditorium
import TrustAllCerts
import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import okhttp3.OkHttpClient
import retrofit2.Call
    import retrofit2.Callback
    import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class AuthActivity : AppCompatActivity() {
        private lateinit var userEmail: EditText
        private lateinit var userPass: EditText
        private lateinit var linkToReg: TextView
        private lateinit var apiService: ApiService

        override fun onCreate(savedInstanceState: Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_auth)

            userEmail = findViewById(R.id.user_login_auth)
            userPass = findViewById(R.id.user_pass_auth)
            linkToReg = findViewById(R.id.link_to_reg)

            val buttonAuth: Button = findViewById(R.id.button_auth)

            val okHttpClient = OkHttpClient.Builder()
                .sslSocketFactory(TrustAllCerts.createSSLSocketFactory(), TrustAllCerts)
                .hostnameVerifier { _, _ -> true }
                .build()

            // Инициализируем Retrofit
            val retrofit = Retrofit.Builder()
                .baseUrl("https://10.0.2.2:7266/") // Замените на ваш URL
                .addConverterFactory(GsonConverterFactory.create())
                .client(okHttpClient)
                .build()

            // Инициализируем ApiService
            apiService = retrofit.create(ApiService::class.java)

            buttonAuth.setOnClickListener {
                authenticateUser()
            }

            linkToReg.setOnClickListener {
                val intent = Intent(this@AuthActivity, MainActivity::class.java)
                startActivity(intent)
            }
        }

        private fun authenticateUser() {
            val email = userEmail.text.toString().trim()
            val pass = userPass.text.toString().trim()

            val credentials = Credentials(email, pass)

            // Вызываем метод аутентификации из ApiService
            apiService.loginUser(credentials).enqueue(object : Callback<AuthResponse> {
                override fun onResponse(call: Call<AuthResponse>, response: Response<AuthResponse>) {
                    if (response.isSuccessful) {
                        val authResponse = response.body()
                        // Успешная аутентификация
                        if (authResponse != null) {
                            saveToken(authResponse.token)
                            Toast.makeText(this@AuthActivity, "Вход выполнен успешно", Toast.LENGTH_SHORT).show()
                            val intent = Intent(this@AuthActivity, RequestActivity::class.java)
                            startActivity(intent)
                        }
                    } else {
                        showToast("Ошибка при регистрации:  ${response.errorBody()?.string()}")}
                }

                override fun onFailure(call: Call<AuthResponse>, t: Throwable) {
                    // Ошибка при выполнении запроса
                    Toast.makeText(this@AuthActivity, "Ошибка при выполнении запроса: ${t.message}", Toast.LENGTH_SHORT).show()
                }
            })
        }

    private fun saveToken(token: String?) {
        val sharedPreferences = getSharedPreferences("user_prefs", MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putString("token", token)
        editor.apply()
    }

    private fun getToken(): String? {
        val sharedPreferences = getSharedPreferences("user_prefs", MODE_PRIVATE)
        return sharedPreferences.getString("token", null)
    }

    private fun showToast(message: String) {
        runOnUiThread {
            Toast.makeText(this@AuthActivity, message, Toast.LENGTH_SHORT).show()
        }
    }

    }







