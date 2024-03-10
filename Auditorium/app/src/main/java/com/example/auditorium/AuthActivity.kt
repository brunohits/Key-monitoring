package com.example.auditorium

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import org.json.JSONObject

class AuthActivity : AppCompatActivity() {
    lateinit var userLogin: EditText
    lateinit var userPass: EditText
    lateinit var linkToReg: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_auth)

        userLogin = findViewById(R.id.user_login_auth)
        userPass = findViewById(R.id.user_pass_auth)
        linkToReg = findViewById(R.id.link_to_reg)

        val buttonAuth: Button = findViewById(R.id.button_auth)
        buttonAuth.setOnClickListener {
            authenticateUser()
        }

        linkToReg.setOnClickListener {
            val intent = Intent(this@AuthActivity, MainActivity::class.java)
            startActivity(intent)
        }
    }

    private fun authenticateUser() {
        val login = userLogin.text.toString().trim()
        val pass = userPass.text.toString().trim()

        val requestBody = JSONObject().apply {
            put("login", login)
            put("password", pass)
        }.toString().toRequestBody("application/json".toMediaTypeOrNull())

        val request = Request.Builder()
            .url("https://localhost:7266/api/account/login")
            .post(requestBody)
            .build()

        val client = OkHttpClient()
        GlobalScope.launch(Dispatchers.IO) {
            try {
                val response = client.newCall(request).execute()
                val responseData = response.body?.string()
                withContext(Dispatchers.Main) {
                    if (response.isSuccessful) {
                        Toast.makeText(this@AuthActivity, "Вход выполнен успешно", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(this@AuthActivity, "Ошибка при входе: $responseData", Toast.LENGTH_SHORT).show()
                    }
                }
            } catch (e: Exception) {
                e.printStackTrace()
                withContext(Dispatchers.Main) {
                    Toast.makeText(this@AuthActivity, "Ошибка при выполнении запроса: ${e.message}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}
