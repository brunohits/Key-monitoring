package com.example.auditorium


import TrustAllCerts
import android.content.ContentValues.TAG
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class RequestActivity : AppCompatActivity() {
    private lateinit var requestList: RecyclerView
    private lateinit var requestAdapter: RequestAdapter
    private lateinit var apiService: ApiService

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_request)

        requestList = findViewById(R.id.request_list)
        requestAdapter = RequestAdapter(emptyList(), this)
        requestList.layoutManager = LinearLayoutManager(this)
        requestList.adapter = requestAdapter

        val okHttpClient = OkHttpClient.Builder()
            .sslSocketFactory(TrustAllCerts.createSSLSocketFactory(), TrustAllCerts)
            .hostnameVerifier { _, _ -> true }
            .build()

        val retrofit = Retrofit.Builder()
            .baseUrl("https://10.0.2.2:7266/") // Замените на ваш URL
            .addConverterFactory(GsonConverterFactory.create())
            .client(okHttpClient)
            .build()

        apiService = retrofit.create(ApiService::class.java)

        val button: Button = findViewById(R.id.button_make_request)
        button.setOnClickListener {
            val intent = Intent(this@RequestActivity, AuditoryActivity::class.java)
            startActivity(intent)
        }


        val call = apiService.getRequests(userId = "ef83867e-dfb6-484b-87fa-069e16bcdf80", status = 0)
        call.enqueue(object : Callback<RequestListResponse> {
            override fun onResponse(call: Call<RequestListResponse>, response: Response<RequestListResponse>) {
                if (response.isSuccessful) {
                    val requestListResponse = response.body()
                    val requests = requestListResponse?.list ?: emptyList()
                    requestAdapter.updateRequests(requests)
                } else {
                    Toast.makeText(this@RequestActivity, "Ошибка при получении списка заявок", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<RequestListResponse>, t: Throwable) {
                // Обработайте ошибку
            }
        })
        }

    }

class RequestAdapter(private var requests: List<RequestItem>, private val context: Context) :
    RecyclerView.Adapter<RequestAdapter.RequestViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RequestViewHolder {
        val itemView = LayoutInflater.from(context).inflate(R.layout.request_in_list, parent, false)
        return RequestViewHolder(itemView)
    }

    override fun onBindViewHolder(holder: RequestViewHolder, position: Int) {
        val request = requests[position]
        holder.bind(request)
    }

    override fun getItemCount(): Int {
        return requests.size
    }

    inner class RequestViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val numberTextView: TextView = itemView.findViewById(R.id.number_of_key)
        private val statusTextView: TextView = itemView.findViewById(R.id.status)
        private val cancelButton: Button = itemView.findViewById(R.id.button_details)

        fun bind(request: RequestItem) {
            numberTextView.text = request.keyNumber
            statusTextView.text = request.status.toString()

            // Обработка нажатия кнопки отмены
            cancelButton.setOnClickListener {
                // Вызов метода для отмены заявки с помощью API
                cancelRequest(request.id)
            }
        }
    }

    // Метод для отмены заявки
    private fun cancelRequest(requestId: String) {

        val okHttpClient = OkHttpClient.Builder()
            .sslSocketFactory(TrustAllCerts.createSSLSocketFactory(), TrustAllCerts)
            .hostnameVerifier { _, _ -> true }
            .build()

        val retrofit = Retrofit.Builder()
            .baseUrl("https://10.0.2.2:7266/") // Замените на ваш URL
            .addConverterFactory(GsonConverterFactory.create())
            .client(okHttpClient)
            .build()

        // Создайте объект apiService с использованием Retrofit
        val apiService = retrofit.create(ApiService::class.java)

        val call: Call<Any> = apiService.deleteApplication(DeleteRequest(requestId))
        call.enqueue(object : Callback<Any> {
            override fun onResponse(call: Call<Any>, response: Response<Any>) {
                if (response.isSuccessful) {
                    // Успешное удаление, обновляем список
                    // TODO: Обновить список заявок после удаления
                } else {
                    // Обработка ошибки при удалении заявки
                }
            }

            override fun onFailure(call: Call<Any>, t: Throwable) {
                // Обработка ошибки при выполнении запроса
            }
        })
    }

    // Метод для обновления списка заявок
    fun updateRequests(newRequests: List<RequestItem>) {
        requests = newRequests
        notifyDataSetChanged()
    }
}
