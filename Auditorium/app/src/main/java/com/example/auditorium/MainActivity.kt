package com.example.auditorium

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.DatePicker
import android.widget.EditText
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.RequestBody.Companion.toRequestBody
import org.json.JSONObject
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class MainActivity : AppCompatActivity() {

    private lateinit var retrofit: Retrofit
    private lateinit var apiService: ApiService

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val userLogin = findViewById<EditText>(R.id.user_login)
        val userEmail = findViewById<EditText>(R.id.user_email)
        val userPass = findViewById<EditText>(R.id.user_pass)
        val userPhone = findViewById<EditText>(R.id.phone_number_edit_text)
        val userRegisterButton = findViewById<Button>(R.id.button_reg)
        val linkToAuth = findViewById<TextView>(R.id.link_to_auth)
        val userBirth = findViewById<DatePicker>(R.id.user_birth)
        val gender = findViewById<Spinner>(R.id.gender_spinner)

        linkToAuth.setOnClickListener {
            val intent = Intent(this@MainActivity, AuthActivity::class.java)
            startActivity(intent)
        }

        retrofit = Retrofit.Builder()
            .baseUrl("https://localhost:7266/") // Замените на ваш URL
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        apiService = retrofit.create(ApiService::class.java)

        userRegisterButton.setOnClickListener {
            val fullName = userLogin.text.toString().trim()
            val email = userEmail.text.toString().trim()
            val password = userPass.text.toString().trim()
            val phoneNumber = userPhone.text.toString().trim()
            val birth =
                "${userBirth.year}-${userBirth.month + 1}-${userBirth.dayOfMonth}"
            val genderUser = gender.selectedItem.toString()

            if (!isValidPhoneNumber(phoneNumber)) {
                showToast("Неправильный формат телефона")
                return@setOnClickListener
            }

            if (!isValidPassword(password)) {
                showToast("Пароль должен содержать не менее 6 символов")
                return@setOnClickListener
            }

            val user = User(
                fullName = fullName,
                email = email,
                password = password,
                birthDate = birth,
                gender = genderUser,
                phoneNumber = phoneNumber,
                facultyId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            )

            registerUser(user)
        }
    }

    private fun registerUser(user: User) {
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val response = apiService.registerUser(user)
                if (response.isSuccessful) {
                    showToast("Пользователь успешно зарегистрирован")
                } else {
                    showToast("Ошибка при регистрации: ${response.message()}")
                }
            } catch (e: Exception) {
                showToast("Ошибка при выполнении запроса: ${e.message}")
            }
        }
    }

    private fun isValidPhoneNumber(phoneNumber: String): Boolean {
        return phoneNumber.matches("^\\+\\d{11}$".toRegex())
    }

    private fun isValidPassword(password: String): Boolean {
        return password.length >= 6
    }

    private fun showToast(message: String) {
        runOnUiThread {
            Toast.makeText(this@MainActivity, message, Toast.LENGTH_SHORT).show()
        }
    }
}
