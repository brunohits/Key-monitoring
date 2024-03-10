package com.example.auditorium

import android.content.Context
import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class AuditoryAdapter(private var auditories: List<Auditory>, var context: Context ): RecyclerView.Adapter<AuditoryAdapter.MyViewHolder>() {

    class MyViewHolder(view: View): RecyclerView.ViewHolder(view){
        val title: TextView = view.findViewById(R.id.number_of_auditory)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
       val view = LayoutInflater.from(parent.context).inflate(R.layout.auditories_list,parent,false)
        return MyViewHolder(view)
    }

    override fun getItemCount(): Int {
        return  auditories.count()
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
       holder.title.text = auditories[position].title
    }


}