package com.example.auditorium

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView

class RequestActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_request)

        val requestList:RecyclerView = findViewById(R.id.request_list)
        val request = arrayListOf<Request>()



        request.add(Request(1,"220 аудитория ","на рассмотрении"))
        request.add(Request(2,"222 аудитория ","одоброено"))
        request.add(Request(3,"227 аудитория ","отклонено"))
        request.add(Request(1,"220 аудитория ","на рассмотрении"))
        request.add(Request(2,"222 аудитория ","одоброено"))
        request.add(Request(3,"227 аудитория ","отклонено"))
        request.add(Request(1,"220 аудитория ","на рассмотрении"))
        request.add(Request(2,"222 аудитория ","одоброено"))
        request.add(Request(3,"227 аудитория ","отклонено"))

        requestList.layoutManager = LinearLayoutManager(this)
        requestList.adapter = RequestAdapter(request,this)

        val  button: Button = findViewById(R.id.button_make_request)

        button.setOnClickListener {
            val intent = Intent(this@RequestActivity, AuditoryActivity::class.java)
            startActivity(intent)
        }

    }
}