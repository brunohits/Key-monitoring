package com.example.auditorium

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView

class KeyActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.keys_list)

        val auditoryList: RecyclerView = findViewById(R.id.key_list)
        val auditory = arrayListOf<Auditory>()

        auditory.add(Auditory(1, "228 key "))
        auditory.add(Auditory(2, "222 key "))
        auditory.add(Auditory(3, "226 key "))
        auditory.add(Auditory(4, "227 key "))
        auditory.add(Auditory(5, "229 key "))




        auditoryList.layoutManager = LinearLayoutManager(this)
        auditoryList.adapter = AuditoryAdapter(auditory, this)



    }
}