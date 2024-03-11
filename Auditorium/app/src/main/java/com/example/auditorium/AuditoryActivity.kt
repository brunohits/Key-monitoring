package com.example.auditorium

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView

class AuditoryActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_auditory)

        val auditoryList: RecyclerView = findViewById(R.id.audi_list)
        val auditory = arrayListOf<Auditory>()

        auditory.add(Auditory(1, "220 аудитория "))
        auditory.add(Auditory(2, "222 аудитория "))
        auditory.add(Auditory(3, "226 аудитория "))
        auditory.add(Auditory(4, "227 аудитория "))
        auditory.add(Auditory(5, "229 аудитория "))
        auditory.add(Auditory(6, "231 аудитория "))
        auditory.add(Auditory(7, "232 аудитория "))
        auditory.add(Auditory(8, "213 аудитория "))
        auditory.add(Auditory(9, "216 аудитория "))



        auditoryList.layoutManager = LinearLayoutManager(this)
        auditoryList.adapter = AuditoryAdapter(auditory, this)

        val button: Button = findViewById(R.id.button_to_booking)
        button.setOnClickListener {
            val intent = Intent(this@AuditoryActivity, BookingActivity::class.java)
            startActivity(intent)
        }

    }
}